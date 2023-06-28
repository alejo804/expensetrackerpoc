import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ExpenseFormComponent } from './expense-form/expense-form.component';


const routes: Routes = [
  { path: '', redirectTo: 'expenses', pathMatch: 'full' },
  { path: 'expenses', component: ExpenseFormComponent },
  // Other routes if needed
];


@NgModule({
  imports: [],
  exports: [RouterModule]
})
export class AppRoutingModule { }
