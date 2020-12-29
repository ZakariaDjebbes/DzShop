import { IReview } from './review';

export interface IProduct {
    id: number;
    name: string;
    description: string;
    price: number;
    pictureUrl: string;
    productType: string;
    productBrand: string;
    reviewsAverage: number;
    reviews: IReview[];
}
