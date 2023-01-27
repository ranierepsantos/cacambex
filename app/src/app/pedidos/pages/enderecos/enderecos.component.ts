import { ClienteService } from './../../../clientes/servicos/cliente.service';
import { Component, Inject, OnInit } from '@angular/core';
import { VisualizarEndereco } from 'src/app/clientes/interfaces/ienderecos';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  templateUrl: './enderecos.component.html',
  styleUrls: ['./enderecos.component.css'],
})
export class EnderecosComponent implements OnInit {
  id: number = 0;
  titulo: string = 'Endereços';
  botaoNovo = false;
  displayedColumns = [
    'logradouro',
    'numero',
    'complemento',
    'bairro',
    'cidade',
    'uf',
    'tipoEndereco',
    'acao',
  ];
  dataSource: VisualizarEndereco[] = [];
  constructor(
    private clienteService: ClienteService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit(): void {
    this.clienteService.ObterClientePorId(this.data.clienteId)
      .subscribe((x) => {
        this.dataSource = x.enderecosEntrega;
      });
  }
}
