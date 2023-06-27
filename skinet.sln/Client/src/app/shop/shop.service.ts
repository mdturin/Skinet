import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IPagination, Pagination } from '../shared/models/pagination';
import { IProductBrand } from '../shared/models/product-brand';
import { IProductType } from '../shared/models/product-type';
import { map, of } from 'rxjs';
import { ShopParams } from '../shared/models/shop-params';
import { IProduct } from '../shared/models/product';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  baseUrl = environment.apiUrl;

  products: IProduct[] = [];
  types: IProductType[] = [];
  brands: IProductBrand[] = [];

  pagination = new Pagination();
  shopParams = new ShopParams();
  productCache = new Map();

  constructor(private http: HttpClient) {}

  getProducts(useCache: boolean) {
    if (useCache === false) {
      this.products = [];
    }

    if (this.productCache.size > 0 && useCache === true) {
      const shopParamsKey = Object.values(this.shopParams).join('-');
      if (this.productCache.has(shopParamsKey)) {
        this.pagination.data = this.productCache.get(shopParamsKey);
        return of(this.pagination);
      }
    }

    let params = this.getParams();
    const endPoint = this.baseUrl + 'products';
    return this.http
      .get<IPagination>(endPoint, { observe: 'response', params })
      .pipe(
        map((response) => {
          const shopParamsKey = Object.values(this.shopParams).join('-');
          this.productCache.set(shopParamsKey, response.body.data);
          this.pagination = response.body;
          return this.pagination;
        })
      );
  }

  setShopParams(params: ShopParams) {
    this.shopParams = params;
  }

  getShopParams() {
    return this.shopParams;
  }

  private getParams() {
    let brandId = this.shopParams.brandId;
    let typeId = this.shopParams.typeId;
    let sort = this.shopParams.sort;
    let pageNumber = this.shopParams.pageNumber.toString();
    let pageSize = this.shopParams.pageSize.toString();
    let search = this.shopParams.search;

    let params = new HttpParams();
    if (brandId !== 0) {
      params = params.append('brandId', brandId.toString());
    }

    if (typeId !== 0) {
      params = params.append('typeId', typeId.toString());
    }

    params = params.append('sort', sort);

    if (pageNumber) {
      params = params.append('pageIndex', pageNumber);
    }

    if (pageSize) {
      params = params.append('pageSize', pageSize);
    }

    if (search) {
      params = params.append('search', search);
    }

    return params;
  }

  getProduct(id: number) {
    let product: IProduct;
    this.productCache.forEach((products: IProduct[]) => {
      product = products.find((p) => p.id === id);
    });

    if (product) {
      return of(product);
    }

    const endPoint = this.baseUrl + 'products/' + id;
    return this.http.get<IProduct>(endPoint);
  }

  getBrands() {
    if (this.brands.length > 0) {
      return of(this.brands);
    }
    const endPoint = this.baseUrl + 'products/brands';
    return this.http.get<IProductBrand[]>(endPoint).pipe(
      map((response) => {
        this.brands = response;
        return response;
      })
    );
  }

  getTypes() {
    if (this.types.length > 0) {
      return of(this.types);
    }
    const endPoint = this.baseUrl + 'products/types';
    return this.http.get<IProductType[]>(endPoint).pipe(
      map((response) => {
        this.types = response;
        return response;
      })
    );
  }
}
