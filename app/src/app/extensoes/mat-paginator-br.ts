import { Injectable } from "@angular/core";
import { MatPaginatorIntl } from "@angular/material/paginator";

@Injectable()
export class MatPaginatorBr extends MatPaginatorIntl {
    itemsPerPageLabel = "Itens por página";
    nextPageLabel = "Próxima página";
    previousPageLabel = "Voltar";

}
