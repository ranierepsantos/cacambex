import { Component, Input, OnInit } from '@angular/core';
import { StatusCacamba } from 'src/app/cacambas/interfaces/status-cacamba';

@Component({
  selector: 'ca-caixa-status-cacamba',
  templateUrl: './caixa-status-cacamba.component.html',
  styleUrls: ['./caixa-status-cacamba.component.css'],
})
export class CaixaStatusCacambaComponent implements OnInit {
  @Input() caixaStatus!: StatusCacamba;
  constructor() { }

  ngOnInit(): void { }
}
