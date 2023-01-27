import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

import { AlterarCacamba, NovaCacamba, VisualizarCacamba } from '../../interfaces/icacamba';

@Component({
  templateUrl: './editar-cacamba.component.html',
  styleUrls: ['./editar-cacamba.component.css'],
})
export class EditarCacambaComponent implements OnInit {
  visualizarCacamba = {} as VisualizarCacamba
  novaCacamba: NovaCacamba = {} as NovaCacamba;
  alterarCacamba: AlterarCacamba = {} as AlterarCacamba;
  titulo: string = 'Nova caçamba';
  desabilitarVolume = false;
  constructor(
    @Inject(MAT_DIALOG_DATA) public x: VisualizarCacamba
  ) { }

  ngOnInit(): void {
    if (this.x) {
      this.desabilitarVolume = true;
      this.titulo = 'Editar Caçamba';
      this.visualizarCacamba.numero = this.x.numero;
      this.visualizarCacamba.volume = this.x.volume;
      this.visualizarCacamba.preco = this.x.preco;
    }
  }

  data() {
    if (this.x) {
      this.alterarCacamba.id = this.x.id;
      this.alterarCacamba.numero = this.visualizarCacamba.numero;
      this.alterarCacamba.volume = this.visualizarCacamba.volume;
      this.alterarCacamba.preco = this.visualizarCacamba.preco;
      return this.alterarCacamba;
    }

    this.novaCacamba.numero = this.visualizarCacamba.numero;
    this.novaCacamba.volume = this.visualizarCacamba.volume;
    this.novaCacamba.preco = this.visualizarCacamba.preco;
    return this.novaCacamba;
  }
}
