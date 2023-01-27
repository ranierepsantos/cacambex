import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'ca-exclusao',
  templateUrl: './popup-confirmacao.component.html',
  styleUrls: ['./popup-confirmacao.component.css']
})
export class PopupConfirmacaoComponent implements OnInit {
  constructor(@Inject(MAT_DIALOG_DATA) public dados: any) { }

  ngOnInit(): void {
  }
}
