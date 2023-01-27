import { Component, Input, OnInit } from '@angular/core';
import { VisualizarCacamba } from 'src/app/cacambas/interfaces/icacamba';

@Component({
  selector: 'ca-add-primeiro-item',
  templateUrl: './add-primeiro-item.component.html',
  styleUrls: ['./add-primeiro-item.component.css']
})
export class AddPrimeiroItemComponent implements OnInit {
  @Input() cacamba!: VisualizarCacamba;

  constructor() { }

  ngOnInit(): void {
  }
}
