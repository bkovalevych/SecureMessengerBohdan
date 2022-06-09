export class UserValue {
    firstName: string;
    lastName: string;
    constructor(init: Partial<UserValue>) {
        Object.assign(this, init);
    }
}
