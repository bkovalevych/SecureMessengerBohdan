export class PagingList<T> {
    totalCount: number;
    take: number;
    skip: number;
    items: T[]
}