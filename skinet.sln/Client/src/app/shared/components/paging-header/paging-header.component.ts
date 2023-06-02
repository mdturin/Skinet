import { Component, Input } from '@angular/core';
import { ShopParams } from '../../models/shop-params';

@Component({
  selector: 'app-paging-header',
  templateUrl: './paging-header.component.html',
  styleUrls: ['./paging-header.component.scss']
})
export class PagingHeaderComponent {
  @Input() pageSize: number;
  @Input() pageNumber: number;
  @Input() totalCount: number;
  
  getstartItemInfo(){
    var result = (this.pageNumber-1) * this.pageSize + 1;
    return result > this.totalCount ? this.totalCount : result;
  }

  getEndItemInfo(){
    var result = this.pageNumber * this.pageSize;
    return result > this.totalCount ? this.totalCount : result;
  }
}
