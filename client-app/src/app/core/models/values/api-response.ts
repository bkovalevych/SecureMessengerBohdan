export interface EmptyApiResponse {
    errors: string[];
    isSucceeded: boolean;
    
}

export interface ApiResponse<TResult> extends EmptyApiResponse {
    result: TResult;
}