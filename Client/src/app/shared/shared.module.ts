import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { PagingHeaderComponent } from './components/paging-header/paging-header.component';
import { PagerComponent } from './components/pager/pager.component';
import { OrderTotalsComponent } from './components/order-totals/order-totals.component';

@NgModule({
  // forRoot cuz the pagination module has its own providers array ,
  // those providers need to be injected at startup
  declarations: [PagingHeaderComponent, PagerComponent, OrderTotalsComponent],
  imports: [CommonModule, PaginationModule.forRoot()],
  exports: [PaginationModule, PagingHeaderComponent, PagerComponent, OrderTotalsComponent],
})
export class SharedModule {}
