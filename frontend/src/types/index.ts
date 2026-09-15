export interface Product {
  id: number;
  code: string;
  name: string;
  description?: string;
}

export interface Material {
  id: number;
  code: string;
  name: string;
  unit: string;
  stock: number;
}

export interface Machine {
  id: number;
  code: string;
  name: string;
  status: number;
  capacity: number;
}

export interface ProductionOrder {
  id: number;
  productId: number;
  quantity: number;
  dueDate: string;
  status: number;
}

export interface BOMItem {
  id: number;
  productId: number;
  componentId: number;
  quantity: number;
}