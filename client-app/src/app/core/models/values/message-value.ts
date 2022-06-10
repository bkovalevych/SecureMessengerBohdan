export class MessageValue {
    id: string;
    fromId: string;
    fromName?: string;
    toId: string;
    toName?: string;
    message: string;
    isUnread?: boolean = false;
    constructor(init: Partial<MessageValue>) {
        Object.assign(this, init);
    }
}
