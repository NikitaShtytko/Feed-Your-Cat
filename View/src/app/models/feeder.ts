import {User} from "./user";
import {Tag} from "./tag";

export class Feeder {
  id: number;
  name: string;
  type: string;
  is_empty: boolean;
  fullness: number;
  is_registered: boolean;
  tags :Tag[];
  user: User;
}
