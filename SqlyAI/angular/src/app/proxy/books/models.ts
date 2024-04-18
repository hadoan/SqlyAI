import type { AuditedEntityDto } from '@abp/ng.core';

export interface BookDto extends AuditedEntityDto<string> {
  name?: string;
  publishDate?: string;
  price: number;
}

export interface CreateUpdateBookDto {
  name: string;
  publishDate: string;
  price: number;
}
