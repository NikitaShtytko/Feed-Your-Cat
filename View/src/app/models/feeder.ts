import {User} from "./user";
import {Tag} from "./tag";

export class Feeder {
  id: number;
  user_id: string;
  type: string;
  status: number;
  empty: boolean;
  log_id: number;
  // tags :Tag[];
  // user_id: User;
}
