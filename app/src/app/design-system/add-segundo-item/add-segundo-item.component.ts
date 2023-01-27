import { Component, Input, OnInit } from '@angular/core';
import { VisualizarCacamba } from 'src/app/cacambas/interfaces/icacamba';

@Component({
  selector: 'ca-add-segundo-item',
  templateUrl: './add-segundo-item.component.html',
  styleUrls: ['./add-segundo-item.component.css']
})
export class AddSegundoItemComponent implements OnInit {
  @Input() cacamba!: VisualizarCacamba;
  constructor() { }

  ngOnInit(): void {

  }
}
