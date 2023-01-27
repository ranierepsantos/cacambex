import { CacambaServico } from '../../../cacambas/servicos/cacamba.service';
import { Component, OnInit } from '@angular/core';
import { VisualizarCacamba } from 'src/app/cacambas/interfaces/icacamba';

@Component({
  templateUrl: './selecionar-cacamba.component.html',
  styleUrls: ['./selecionar-cacamba.component.css'],
})
export class SelecionarCacambaComponent implements OnInit {
  displayedColumns = ['numero', 'volume', 'acao'];
  titulo: string = 'Caçambas';
  cacambasDisponiveis: VisualizarCacamba[] = [];
  constructor(private cacambaServico: CacambaServico) { }

  ngOnInit(): void {
    this.cacambaServico
      .obterCacambasDisponiveis()
      .subscribe((x) => {
        this.cacambasDisponiveis = x;
      });
  }
}
