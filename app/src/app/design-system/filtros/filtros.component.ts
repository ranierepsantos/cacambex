import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'ca-filtros',
  templateUrl: './filtros.component.html',
  styleUrls: ['./filtros.component.css']
})
export class FiltrosComponent implements OnInit {
  @Input() titulo!: string;
  @Input() nomeCliente: string = '';
  @Input() documentoCliente!: string;
  @Input() notaFiscal!: string;
  @Input() numeroCTR!: string;
  @Input() sort!: string;
  @Input() dataInicio: Date = new Date();
  @Input() dataFim: Date = new Date();
  @Input() filtroPorData: boolean = false;
  @Input() isAdmin!: boolean;

  @Output() filtrar = new EventEmitter();
  @Output() nomeClienteChange = new EventEmitter<string>();
  @Output() documentoClienteChange = new EventEmitter<string>();
  @Output() notaFiscalChange = new EventEmitter<string>();
  @Output() numeroCTRChange = new EventEmitter<string>();
  @Output() sortChange = new EventEmitter<string>();
  @Output() dataInicioChange = new EventEmitter<Date>();
  @Output() dataFimChange = new EventEmitter<Date>();
  @Output() filtroPorDataChange = new EventEmitter<boolean>();

  constructor() { }

  ngOnInit(): void {
  }

  aplicarFiltros() {
    this.filtrar.emit();
  }
}
