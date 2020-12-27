import {User} from "./user";
import {Tag} from "./tag";
import {Schedule} from "./schedule";

export class Feeder {
  id: number;
  name: string;
  type: string;
  is_Empty: boolean;
  fullness: number;
  is_registered: boolean;
  tags :Tag[];
  user: User;
  schedules: Schedule[];
}
