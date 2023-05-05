import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { PopupConfirmacaoComponent } from 'src/app/design-system/popup-confirmacao/popup-confirmacao.component';
import { PopupInformativoComponent } from 'src/app/design-system/popup-informativo/popup-informativo.component';
import { IStatusPedidoOmie} from 'src/app/pedidos/interfaces/iomie-response';

import { VisualizarTipoCacamba, AlterarTipoCacamba, IPrecoFaixaCep } from 'src/app/tipo-cacamba/interfaces/itipo-cacamba';
import { TipoCacambaServico } from 'src/app/tipo-cacamba/servicos/tipo-cacamba.service';
@Component({
  selector: 'ca-editar-tipo',
  templateUrl: './editar-tipo.component.html',
  styleUrls: ['./editar-tipo.component.css']
})
export class EditarTipoComponent implements OnInit {
  VisualizarTipoCacamba = {} as VisualizarTipoCacamba
  tipoCacamba: AlterarTipoCacamba = {} as AlterarTipoCacamba;
faixaPreco: IPrecoFaixaCep = {id: 0, cepInicial: "", cepFinal: "", preco: 0.00} as IPrecoFaixaCep;
  titulo: string = 'Editar Tipo Caçamba';
  desabilitarVolume = false;
  faixaPrecoCepId: number = 0;
  dataSource = new MatTableDataSource<IPrecoFaixaCep>([]);
  displayedColumns = ['cepInicial', 'cepFinal','preco', 'acao'];
  constructor(
    private apiService: TipoCacambaServico,
    private dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public tipo: VisualizarTipoCacamba,
  ) { }

  ngOnInit(): void {
    if (this.tipo) {
      this.titulo = 'Editar Tipo Caçamba';
      this.obterDados(this.tipo.id);
    }else {
      this.titulo = 'Novo Tipo Caçamba';
    }
    
  }

  obterDados (tipoCacambaId: number) {
   this.apiService.obterPorId(tipoCacambaId)
    .subscribe((data) => {
      this.tipoCacamba = data;
      this.tipoCacamba.preco = parseFloat(this.tipoCacamba.preco).toFixed(2) 
      this.dataSource.data = [...this.tipoCacamba.precoFaixaCep].sort((a,b) => (a.cepInicial > b.cepInicial) ? 1 : ((b.cepInicial > a.cepInicial) ? -1 : 0))
    });
  }

  editarFaixaPreco (faixa: IPrecoFaixaCep)
  {
    this.faixaPreco = {...faixa};
    this.faixaPreco.preco = parseFloat(faixa.preco).toFixed(2) 
  }

  excluirFaixaPreco (faixa: IPrecoFaixaCep)
  {

    const dialogRef = this.dialog.open(PopupConfirmacaoComponent, {
      width: '250px',
      data: { titulo: 'a faixa', identificador: `${faixa.cepInicial} - ${faixa.cepFinal}` }
    })
    dialogRef.afterClosed().subscribe((x) => {
      if (x) {
        let index = this.tipoCacamba.precoFaixaCep.findIndex(c => c.id == faixa.id);
        if (index != -1) {
          this.tipoCacamba.precoFaixaCep.splice(index, 1)
        }
        this.dataSource.data = [...this.tipoCacamba.precoFaixaCep].sort((a,b) => (a.cepInicial > b.cepInicial) ? 1 : ((b.cepInicial > a.cepInicial) ? -1 : 0))    
      }
    })
    
  }

  salvarFaixaPreco () {

    //validar 
    if (this.faixaPreco.cepFinal =='' || this.faixaPreco.cepInicial == '' ||
        this.faixaPreco.cepFinal < this.faixaPreco.cepInicial ||
        this.faixaPreco.preco =='' || parseFloat(this.faixaPreco.preco) <=0 )
    {
      let dados = {nNfse: "", mensagens: []} as IStatusPedidoOmie
      dados.mensagens.push({cCorrecao: "Atenção", cDescricao: "Dados inválidos!" });
      this.dialog.open(PopupInformativoComponent, {
        data: { info: {dados: [dados]} }
      });
      this.faixaPreco = {id: 0, cepInicial: "", cepFinal: "", preco: 0.00} as IPrecoFaixaCep;
      return  
    }

    if (this.faixaPreco.id == 0) {
      this.faixaPrecoCepId --;
      this.faixaPreco.id = this.faixaPrecoCepId;
      this.tipoCacamba.precoFaixaCep.push({...this.faixaPreco});
    }else {
      let index = this.tipoCacamba.precoFaixaCep.findIndex(c => c.id == this.faixaPreco.id);
      if (index != -1) {
        this.tipoCacamba.precoFaixaCep[index] = {...this.faixaPreco}
      }
    }
    this.faixaPreco = {id: 0, cepInicial: "", cepFinal: "", preco: 0.00} as IPrecoFaixaCep;
    this.dataSource.data = [...this.tipoCacamba.precoFaixaCep].sort((a,b) => (a.cepInicial > b.cepInicial) ? 1 : ((b.cepInicial > a.cepInicial) ? -1 : 0))
  }



  salvarDados () {
    return this.tipoCacamba;
  }
}
