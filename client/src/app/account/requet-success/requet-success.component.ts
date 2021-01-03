import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-requet-success',
  templateUrl: './requet-success.component.html',
  styleUrls: ['./requet-success.component.scss']
})
export class RequetSuccessComponent implements OnInit {
  email: string;

  constructor(private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    const state = navigation && navigation.extras && navigation.extras.state;
    if (state) {
      this.email = state as unknown as string;
    } else {
      this.router.navigateByUrl('/');
    }
   }

   ngOnInit(): void
   {

   }
}
