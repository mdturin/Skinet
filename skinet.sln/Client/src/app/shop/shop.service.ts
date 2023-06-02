import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IPagination } from '../shared/models/pagination';
import { IProductBrand } from '../shared/models/product-brand';
import { IProductType } from '../shared/models/product-type';
import { map } from 'rxjs';
import { ShopParams } from '../shared/models/shop-params';

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) {}

  getProducts(shopParams: ShopParams) {
    let params = this.getParams(shopParams);
    const endPoint = this.baseUrl + 'products';
    return this.http
      .get<IPagination>(endPoint, { observe: 'response', params })
      .pipe(
        map((response) => {
          return response.body;
        })
      );
  }

  private getParams(shopParams: ShopParams) {
    let brandId = shopParams.brandId;
    let typeId = shopParams.typeId;
    let sort = shopParams.sort;
    let pageNumber = shopParams.pageNumber.toString();
    let pageSize = shopParams.pageSize.toString();
    let search = shopParams.search;

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

    if(search) {
      params = params.append('search', search);
    }

    return params;
  }

  getBrands() {
    const endPoint = this.baseUrl + 'products/brands';
    return this.http.get<IProductBrand[]>(endPoint);
  }

  getTypes() {
    const endPoint = this.baseUrl + 'products/types';
    return this.http.get<IProductType[]>(endPoint);
  }
}
