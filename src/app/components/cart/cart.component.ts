import { Component, OnInit, ViewChild } from '@angular/core';
import { CartService } from '../../services/cart.service';
import { ICartItemRead, ICartItemUpdate, IBulkCheckoutDto } from '../../models/cart';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastService } from '../../services/toast.service';
import { FormsModule } from '@angular/forms';
import { CustomerNavComponent } from "../customer-nav/customer-nav.component";
import { CommonModule } from '@angular/common';
import { OrderService } from '../../services/order.service';
import { OrderCreate } from '../../models/order';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [FormsModule, CustomerNavComponent, CommonModule],
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  form: boolean = false;
  buttonname: string = 'Order Now';
  productId!: number;
  items: ICartItemRead[] = [];
  selected: Set<number> = new Set<number>();
  customerId: number = 0;
  receiverName = '';
  receiverContact = '';
  address = '';
  postalCode = '';
  selectedItems: any;
  deliveryDate: Date = new Date();

  constructor(
    private route: ActivatedRoute,
    private cartService: CartService,
    private router: Router,
    private toast: ToastService,
    private orderService: OrderService,
  ) { }

  ngOnInit(): void {
    const future = new Date();
    future.setDate(future.getDate() + 7);
    this.deliveryDate = future;
    this.productId = Number(this.route.snapshot.paramMap.get('id'));
    this.customerId = Number(localStorage.getItem('userId') || 0);
    if (!this.customerId) {
      this.toast.show('Please login', 'error');
      this.router.navigate(['/login']);
      return;
    }
    this.loadCart();
  }

  Order() {
    if (this.selected.size === 0) {
      this.toast.show('Select at least one product to order.', 'error');
      return;
    }
    if (this.form == false) {
      this.form = true;
      this.buttonname = 'Back';
    }
    else {
      this.form = false;
    }
  }

  get SeeselectedItems(): ICartItemRead[] {
    return this.items.filter(i => this.selected.has(i.id));
  }

  loadCart() {
    this.cartService.getCart(this.customerId).subscribe({
      next: items => this.items = items,
      error: err => {
        console.error(err);
        this.toast.show('Failed to load cart', 'error');
      }
    });
  }

  toggleSelect(itemId: number) {
    if (this.selected.has(itemId)) this.selected.delete(itemId);
    else this.selected.add(itemId);
  }

  updateQuantity(item: ICartItemRead, delta: number) {
    const newQty = item.quantity + delta;
    if (newQty < 1) return;
    const dto: ICartItemUpdate = { id: item.id, quantity: newQty };
    this.cartService.updateItem(dto).subscribe({
      next: () => this.loadCart(),
      error: (err) => {
        console.error(err);
        this.toast.show('Failed to update quantity', 'error');
      }
    });
  }

  remove(itemId: number) {
    this.cartService.removeItem(itemId).subscribe({
      next: () => {
        // this.toast.show('Item remove from cart successfully!', 'success');
        this.loadCart();
      },
      error: (err) => {
        console.error(err);
        this.toast.show('Failed to place order', 'error');
      }
    });
  }

  getSelectedTotal(): number {
    return this.items
      .filter(i => this.selected.has(i.id))
      .reduce((sum, i) => sum + (i.unitPrice * i.quantity), 0);
  }


  checkoutSelected(form: any) {
    if (form.invalid) {
      this.toast.show('Please fill all required fields correctly!', 'error');
      form.form.markAllAsTouched();
      return;
    }

    if (this.selected.size === 0) {
      this.toast.show('Select at least one product to order.', 'error');
      return;
    }

    const selectedItems = this.items.filter(i => this.selected.has(i.id));

    selectedItems.forEach(item => {
      const orderData: OrderCreate = {
        customerId: this.customerId,
        customerName: '',
        productId: item.productId,
        productName: item.productName!,
        quantity: item.quantity,
        totalPrice: item.totalPrice,
        receiverName: this.receiverName,
        receiverContactNumber: this.receiverContact,
        orderAddress: this.address,
        postalCode: this.postalCode
      };

      this.orderService.createOrder(orderData).subscribe({

        next: () => {

          this.toast.show(`Order placed for ${item.productName}`, 'success');
          this.remove(item.id);
            this.router.navigate(['/customer-order']);
        },
        error: (err) => {
          console.error(err);
          this.toast.show(`Failed to order ${item.productName}`, 'error');
        }
      });
    });

  
  }
}
