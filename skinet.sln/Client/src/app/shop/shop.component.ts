import {
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { IProduct } from '../shared/models/product';
import { ShopService } from './shop.service';
import { IProductBrand } from '../shared/models/product-brand';
import { IProductType } from '../shared/models/product-type';
import { ShopParams } from '../shared/models/shop-params';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss'],
})
export class ShopComponent implements OnInit, OnDestroy {
  notifier = new Subject<void>();

  @ViewChild('searchBox', { static: false }) searchBox: ElementRef;

  products: IProduct[];
  types: IProductType[];
  brands: IProductBrand[];

  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low to High', value: 'priceAsc' },
    { name: 'Price: High to Low', value: 'priceDesc' },
  ];

  totalCount: number = 0;
  shopParams: ShopParams;

  constructor(private shopService: ShopService) {
    this.shopParams = this.shopService.getShopParams();
  }

  ngOnDestroy(): void {
    this.notifier.next();
    this.notifier.complete();
  }

  ngOnInit() {
    this.getProducts(true);
    this.getBrands();
    this.getTypes();
  }

  getProducts(useCache: boolean = false) {
    this.shopService
      .getProducts(useCache)
      .pipe(takeUntil(this.notifier))
      .subscribe({
        next: (response: any) => {
          this.products = response.data;
          this.totalCount = response.count;
        },
        error: (error) => console.error(error),
        complete: () => console.info('completed get products from server.'),
      });
  }

  getBrands() {
    this.shopService
      .getBrands()
      .pipe(takeUntil(this.notifier))
      .subscribe({
        next: (response) =>
          (this.brands = [{ id: 0, name: 'All' }, ...response]),
        error: (error) => console.error(error),
        complete: () => console.info('completed get brands from server.'),
      });
  }

  getTypes() {
    this.shopService
      .getTypes()
      .pipe(takeUntil(this.notifier))
      .subscribe({
        next: (response) =>
          (this.types = [{ id: 0, name: 'All' }, ...response]),
        error: (error) => console.error(error),
        complete: () => console.info('completed get types from server.'),
      });
  }

  onBrandSelected(brandId: number) {
    const params = this.shopService.getShopParams();
    params.brandId = brandId;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);
    this.getProducts();
  }

  onTypeSelected(typeId: number) {
    const params = this.shopService.getShopParams();
    params.typeId = typeId;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);
    this.getProducts();
  }

  onSortSelected(sort: string) {
    const params = this.shopService.getShopParams();
    params.sort = sort;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);
    this.getProducts();
  }

  onPageChanged(pageNumber: any) {
    const params = this.shopService.getShopParams();
    if (params.pageNumber === pageNumber) return;
    params.pageNumber = pageNumber;
    this.shopService.setShopParams(params);
    this.getProducts(true);
  }

  onSearch() {
    const params = this.shopService.getShopParams();
    params.search = this.searchBox.nativeElement.value;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);
    this.getProducts();
  }

  onReset() {
    this.searchBox.nativeElement.value = '';
    this.shopParams = new ShopParams();
    this.shopService.setShopParams(this.shopParams);
    this.getProducts();
  }
}
