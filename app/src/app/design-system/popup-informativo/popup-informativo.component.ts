import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { of } from 'rxjs';
import { IStatusPedidoOmie } from 'src/app/pedidos/interfaces/iomie-response';

@Component({
  selector: 'ca-popup-informativo',
  templateUrl: './popup-informativo.component.html',
  styleUrls: ['./popup-informativo.component.css']
})
export class PopupInformativoComponent implements OnInit {
  constructor(@Inject(MAT_DIALOG_DATA) public info: any) { }

  cCorrecao = "";
  cDescricao = "";
  ngOnInit(): void {
    this.info.info.dados.forEach((element: IStatusPedidoOmie) => {
      if (element.mensagens[0].cCorrecao || element.mensagens[0].cDescricao) {
        this.cCorrecao = element.mensagens[0].cCorrecao
        this.cDescricao = element.mensagens[0].cDescricao
      } return of();
    })
  }
}
