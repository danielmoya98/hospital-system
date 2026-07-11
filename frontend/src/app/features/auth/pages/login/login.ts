import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule], // Importante para usar [formGroup]
  templateUrl: './login.html',
})
export class Login{
  loginForm: FormGroup;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private router: Router,
  ) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  onSubmit() {
    if (this.loginForm.invalid) return;

    this.isLoading = true;

    // TODO: Conectar con AuthService hacia .NET
    // Por ahora, simulamos 1 segundo de carga y redirigimos al Layout Principal
    setTimeout(() => {
      this.isLoading = false;
      this.router.navigate(['/app']);
    }, 1000);
  }
}
