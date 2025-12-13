import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule, FormGroup, AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { ToastService } from '../../services/toast.service';

@Component({
  selector: 'app-customer-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './customer-register.component.html',
  styleUrls: ['./customer-register.component.css']
})
export class CustomerRegisterComponent implements OnInit {

  registerForm!: FormGroup;
  submitted = false;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router,
    private toast: ToastService
  ) { }

  ngOnInit(): void {
    this.registerForm = this.fb.group(
      {
        customerName: ['', [Validators.required]],
        email: ['', [Validators.required, Validators.email]],
        contact: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
        gender: ['', Validators.required],
        city: ['', Validators.required],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', Validators.required]
      },
      { validators: this.passwordMatchValidator('password', 'confirmPassword') }
    );
  }

  passwordMatchValidator(password: string, confirmPassword: string): ValidatorFn {
    return (formGroup: AbstractControl): ValidationErrors | null => {
      const pass = formGroup.get(password);
      const confirm = formGroup.get(confirmPassword);

      if (!pass || !confirm) return null;

      if (pass.value !== confirm.value) {
        confirm.setErrors({ passwordMismatch: true });
      } else {
        confirm.setErrors(null);
      }

      return null;
    };
  }

  get f() {
    return this.registerForm.controls;
  }

  onSubmit() {
    this.submitted = true;

    if (this.registerForm.invalid) {
      this.toast.show('Please fill all fields correctly!', 'error');
      return;
    }

    const customerData = {
      customerName: this.registerForm.value.customerName,
      email: this.registerForm.value.email,
      contact: this.registerForm.value.contact,
      gender: this.registerForm.value.gender,
      city: this.registerForm.value.city,
      password: this.registerForm.value.password,
      confirmPassword: this.registerForm.value.confirmPassword,
      createdOn: new Date()
    };

    this.http.post('https://localhost:7011/api/Customers', customerData).subscribe({
      next: () => {
        this.toast.show('Customer Registered Successfully', 'success');
        this.router.navigateByUrl('/login');
        this.registerForm.reset();
        this.submitted = false;
      },
      error: (err) => {
        console.error(err);

        if (err.status === 409) {
          this.registerForm.get('email')?.setErrors({ emailExists: true });
          this.toast.show('Email already exists!', 'error');
          return;
        }
        this.toast.show('Something went wrong. Try again!', 'error');
      }
    });
  }
}
