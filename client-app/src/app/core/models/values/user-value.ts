export class UserValue {
    firstName: string;
    lastName: string;
    userName: string;
    id: string;
    email: string;
    publicKey?: string;
    
    constructor(init: Partial<UserValue>) {
        Object.assign(this, init);
    }
}
