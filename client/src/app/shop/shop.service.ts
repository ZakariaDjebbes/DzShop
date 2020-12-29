import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IBrand } from '../shared/models/brand';
import { IPagination } from '../shared/models/pagination';
import { IType } from '../shared/models/productType';
import { map } from 'rxjs/operators';
import { ReviewParams, ShopParams } from '../shared/models/shopParams';
import { IProduct } from '../shared/models/product';
import { environment } from 'src/environments/environment';
import { IReview } from '../shared/models/review';
import { IReviewToCreate } from '../shared/models/reviewToCreate';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  public getProducts(shopParams: ShopParams): Observable<IPagination<IProduct>> {
    let params = new HttpParams();

    if (shopParams.brandId !== 0) {
      params = params.append('brandId', shopParams.brandId.toString());
    }

    if (shopParams.typeId !== 0) {
      params = params.append('typeId', shopParams.typeId.toString());
    }

    if (shopParams.search)
    {
      params = params.append('search', shopParams.search);
    }

    params = params.append('sort', shopParams.sort);
    params = params.append('pageIndex', shopParams.pageNumber.toString());
    params = params.append('pageSize', shopParams.pageSize.toString());

    return this.http.get<IPagination<IProduct>>(this.baseUrl + 'products', { observe: 'response', params })
      .pipe(
        map(response => {
          return response.body;
        })
      );
  }

  public getBrands(): Observable<IBrand[]> {
    return this.http.get<IBrand[]>(this.baseUrl + 'products/brands');
  }

  public getTypes(): Observable<IType[]> {
    return this.http.get<IType[]>(this.baseUrl + 'products/types');
  }

  // tslint:disable-next-line: typedef
  public getProduct(id: number){
    return this.http.get<IProduct>(this.baseUrl + 'products/' + id);
  }

  public getReviewsOfProduct(id: number, reviewParams: ReviewParams): Observable<IPagination<IReview>>{
    let params = new HttpParams();
    params = params.append('pageIndex', reviewParams.pageNumber.toString());
    params = params.append('pageSize', reviewParams.pageSize.toString());
    return this.http.get<IPagination<IReview>>(this.baseUrl + 'products/reviews/' + id, { observe: 'response', params })
    .pipe(
      map(response => {
        return response.body;
      })
    );
  }

  // tslint:disable-next-line: typedef
  public reviewProduct(review: IReviewToCreate){
    return this.http.post(this.baseUrl + 'products/review', review);
  }
}
