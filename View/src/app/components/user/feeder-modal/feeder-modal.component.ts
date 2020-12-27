import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {ConfirmDialogComponent} from "../../admin/confirm-dialog/confirm-dialog.component";
import {CookieService} from "../../../services/cookie/cookie.service";
import {Feeder} from "../../../models/feeder";
import {FeederService} from "../../../services/feeder/feeder.service";
import {Subscription} from "rxjs";
import {FormControl, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-feeder-modal',
  templateUrl: './feeder-modal.component.html',
  styleUrls: ['./feeder-modal.component.css']
})
export class FeederModalComponent implements OnInit {
  public feeder: Feeder;
  public is_exist = false;
  public empty;
  time: any;
  newTime: boolean;

  public subscriptions: Subscription[] = [];

  constructor(
    private _authCookie: CookieService,
    private dialogRef: MatDialogRef<ConfirmDialogComponent>,
    private feederService: FeederService,
    @Inject(MAT_DIALOG_DATA) data
  ) {
    this.is_exist = data.data !== 'request';

    if (this.is_exist) {
      this.feeder = data.data;
      switch (this.feeder.is_Empty) {
        case false:
          this.empty = "Not empty";
          break;
        case true:
          this.empty = "Empty"
          break;
      }
    }
    else{
      this.feeder = new Feeder();
    }
  }

  form: FormGroup = new FormGroup({
    name: new FormControl('', [
      Validators.required,
      Validators.pattern('^[a-zA-Zа-яА-Я\'_0-9]{4,40}$'),
    ]),

    type: new FormControl('', [
      Validators.required,
      Validators.pattern('^[a-zA-Zа-яА-Я\'_0-9]{4,40}$'),
    ]),
  });

  tag: FormGroup = new FormGroup({
    tag: new FormControl('', [
      Validators.required,
      Validators.pattern('^[a-zA-Zа-яА-Я\'_0-9]{2,10}$'),
    ])
  });


  ngOnInit(): void {

  }

  fill() {
    this.subscriptions.push(this.feederService.fillTheFeeder(this.feeder.id).subscribe(response => {
      this.feeder.fullness = response;
      this.empty = 'Full';
    }));
  }

  feed() {
    this.subscriptions.push(this.feederService.feedTheCat(this.feeder.id).subscribe(response => {
      this.feeder.fullness = response;
      if (response == 0){
        this.empty = 'Empty';
      }
      else if(response == 100){
        this.empty = 'Full';
      }
      else {
        this.empty = 'Not empty';
      }
    }));
  }

  save() {
    this.feeder.type = this.form.controls.type.value;
    this.feeder.name = this.form.controls.name.value;

    this.subscriptions.push(this.feederService.newFeeder(this.feeder).subscribe(response => {
      this.dialogRef.close(true);
    }));
  }

  close() {
    this.dialogRef.close(false);
  }

  newTag() {
    const tag = {
      id: this.feeder.id,
      tag: this.tag.controls.tag.value
    }

    console.log(tag);

    this.subscriptions.push(this.feederService.newTag(tag).subscribe(response => {
      this.feeder = response;
    }));
  }

  deleteTag(id: number) {
    this.subscriptions.push(this.feederService.deleteTag(id).subscribe(response => {
      this.feeder = response;
    }));
  }

  newSchedule(){
    const schedule = {
      id: this.feeder.id,
      time: this.time,
    }

    this.subscriptions.push(this.feederService.newSchedule(schedule).subscribe(response => {
      this.feeder = response;
    }));
  }

  deleteSchedule(id: number){
    this.subscriptions.push(this.feederService.deleteSchedule(id).subscribe(response => {
      this.feeder = response;
    }));
  }
}
