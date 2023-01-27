import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'ca-cabecalho',
  templateUrl: './cabecalho.component.html',
  styleUrls: ['./cabecalho.component.css']
})
export class CabecalhoComponent implements OnInit {
  @Input() desabilitado!: boolean;
  @Input() mostrarBotao = true;
  @Input() mostrarSetaVoltar = false;
  @Input() titulo!: string;
  @Input() nomeBotao: string = 'Novo';
  @Output() novoEventoAbrir: EventEmitter<void> = new EventEmitter();
  @Output() acaoVoltar: EventEmitter<void> = new EventEmitter();

  constructor() {}

  ngOnInit(): void {}
}
