import { MessageValue } from "./message-value";

export class ChatValue {
    id: string;
    name: string;
    countOfUnreadMessages?: number;
    lastMessage?: MessageValue;
    constructor(init: Partial<ChatValue>) {
        Object.assign(this, init);
    }
}
