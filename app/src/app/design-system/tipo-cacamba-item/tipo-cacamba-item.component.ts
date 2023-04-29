import { Component, Input, OnInit } from '@angular/core';
import { VisualizarTipoCacamba } from 'src/app/tipo-cacamba/interfaces/itipo-cacamba';

@Component({
  selector: 'tipo-cacamba-item',
  templateUrl: './tipo-cacamba-item.component.html',
  styleUrls: ['./tipo-cacamba-item.component.css']
})
export class TipoCacambaItem implements OnInit {
  @Input() cacamba!: VisualizarTipoCacamba;

  constructor() { }

  ngOnInit(): void {
  }
}
