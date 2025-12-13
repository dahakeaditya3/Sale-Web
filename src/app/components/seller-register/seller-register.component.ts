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
  templateUrl: './seller-register.component.html',
  styleUrls: ['./seller-register.component.css']
})
export class SellerRegisterComponent implements OnInit {
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
        sellerName: ['', [Validators.required]],
        storeName: ['', [Validators.required]],
        email: ['', [Validators.required, Validators.email]],
        contact: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
        state: ['', Validators.required],
        city: ['', Validators.required],
        address: ['', Validators.required],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', Validators.required]
      },
      {
        validators: this.passwordMatchValidator('password', 'confirmPassword')
      }
    );
  }

  passwordMatchValidator(password: string, confirmPassword: string): ValidatorFn {
    return (formGroup: AbstractControl): ValidationErrors | null => {
      const passControl = formGroup.get(password);
      const confirmControl = formGroup.get(confirmPassword);

      if (!passControl || !confirmControl) return null;

      if (confirmControl.errors && !confirmControl.errors['passwordMismatch']) {
        return null;
      }

      if (passControl.value !== confirmControl.value) {
        confirmControl.setErrors({ passwordMismatch: true });
      } else {
        confirmControl.setErrors(null);
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
      this.toast.show('Please fill all required fields!', 'error');
      return;
    }

    const sellerData = {
      sellerName: this.registerForm.value.sellerName,
      storeName: this.registerForm.value.storeName,
      email: this.registerForm.value.email,
      contact: this.registerForm.value.contact,
      state: this.registerForm.value.state,
      city: this.registerForm.value.city,
      address: this.registerForm.value.address,
      password: this.registerForm.value.password,
      confirmPassword: this.registerForm.value.confirmPassword,
      createdOn: new Date()
    };

    this.http.post('https://localhost:7011/api/Sellers', sellerData).subscribe({
      next: () => {
        this.toast.show('Seller Registered Successfully 🎉', 'success');
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
