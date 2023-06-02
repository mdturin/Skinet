import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { IProduct } from '../shared/models/product';
import { ShopService } from './shop.service';
import { IProductBrand } from '../shared/models/product-brand';
import { IProductType } from '../shared/models/product-type';
import { ShopParams } from '../shared/models/shop-params';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss'],
})
export class ShopComponent implements OnInit {

  @ViewChild('searchBox', {static: false}) searchBox: ElementRef;

  products: IProduct[];
  types: IProductType[];
  brands: IProductBrand[];

  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low to High', value: 'priceAsc' },
    { name: 'Price: High to Low', value: 'priceDesc' },
  ];

  totalCount: number = 0;
  shopParams: ShopParams = new ShopParams();

  constructor(private shopService: ShopService) {}

  ngOnInit() {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  getProducts() {
    this.shopService.getProducts(this.shopParams).subscribe({
      next: (response) => {
        this.products = response.data;
        this.totalCount = response.count;
        this.shopParams.pageSize = response.pageSize;
        this.shopParams.pageNumber = response.pageIndex;
      },
      error: (error) => console.error(error),
      complete: () => console.info('completed get products from server.'),
    });
  }

  getBrands() {
    this.shopService.getBrands().subscribe({
      next: (response) => this.brands = [{id: 0, name: 'All'}, ...response],
      error: (error) => console.error(error),
      complete: () => console.info('completed get brands from server.'),
    });
  }

  getTypes() {
    this.shopService.getTypes().subscribe({
      next: (response) => this.types = [{id: 0, name: 'All'}, ...response],
      error: (error) => console.error(error),
      complete: () => console.info('completed get types from server.'),
    });
  }

  onBrandSelected(brandId: number) {
    this.shopParams.brandId = brandId;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onTypeSelected(typeId: number) {
    this.shopParams.typeId = typeId;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onSortSelected(sort: string) {
    this.shopParams.sort = sort;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onPageChanged(pageNumber: any){
    if(this.shopParams.pageNumber === pageNumber) return;
    this.shopParams.pageNumber = pageNumber;
    this.getProducts();
  }

  onSearch() {
    this.shopParams.search = this.searchBox.nativeElement.value;
    this.getProducts();
  }

  onReset() {
    this.searchBox.nativeElement.value = '';
    this.shopParams = new ShopParams();
    this.getProducts();
  }
}