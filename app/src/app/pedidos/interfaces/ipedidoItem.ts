import { VisualizarCacamba } from 'src/app/cacambas/interfaces/icacamba';

import { Concluido, CTR, Entregue, Recolher } from './ieventos';

export interface VisualizarPedidoItem {
  id: number;
  volumeCacamba: string;
  cacamba: VisualizarCacamba;
  valorUnitario: number;
  ctr: CTR;
  recolher: Recolher;
  entregue: Entregue;
  concluido: Concluido;
}

