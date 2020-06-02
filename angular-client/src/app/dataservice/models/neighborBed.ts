export class neighborBed {
	ownBedId: string;
	neighborBedId: string[];

	constructor(ownBedId: string, neighborBedId: string[]){
		this.ownBedId = ownBedId;
		this.neighborBedId = neighborBedId;
	}

  // Inititise all beds neighbors
  public static initNeighborSet() : Array<neighborBed> {
  	// Pod A
    let itemA1:neighborBed = new neighborBed("A1",["A2","A12"]);
    let itemA2:neighborBed = new neighborBed("A2",["A1","A3"]);
    let itemA3:neighborBed = new neighborBed("A3",["A2","A4"]);
    let itemA4:neighborBed = new neighborBed("A4",["A3","A5"]);
    let itemA5:neighborBed = new neighborBed("A5",["A4","A6"]);
    let itemA6:neighborBed = new neighborBed("A6",["A5","A7"]);
    let itemA7:neighborBed = new neighborBed("A7",["A6","A8"]);
    let itemA8:neighborBed = new neighborBed("A8",["A7"]);
    let itemA9:neighborBed = new neighborBed("A9",["A10"]);
    let itemA10:neighborBed = new neighborBed("A10",["A9","A11"]);
    let itemA11:neighborBed = new neighborBed("A11",["A10","A12"]);
    let itemA12:neighborBed = new neighborBed("A12",["A11","A1"]);

    // Pod B
    let itemB1:neighborBed = new neighborBed("B1",["B2"]);
    let itemB2:neighborBed = new neighborBed("B2",["B1","B3"]);
    let itemB3:neighborBed = new neighborBed("B3",["B2","B4"]);
    let itemB4:neighborBed = new neighborBed("B4",["B3","B5"]);
    let itemB5:neighborBed = new neighborBed("B5",["B4","B6"]);
    let itemB6:neighborBed = new neighborBed("B6",["B5","B7"]);
    let itemB7:neighborBed = new neighborBed("B7",["B6","B8"]);
    let itemB8:neighborBed = new neighborBed("B8",["B7","B9"]);
    let itemB9:neighborBed = new neighborBed("B9",["B8","B10"]);
    let itemB10:neighborBed = new neighborBed("B10",["B9"]);

    // Pod C
    let itemC1:neighborBed = new neighborBed("C1",["C2"]);
    let itemC2:neighborBed = new neighborBed("C2",["C1","C3"]);
    let itemC3:neighborBed = new neighborBed("C3",["C2","C4"]);
    let itemC4:neighborBed = new neighborBed("C4",["C3","C5"]);
    let itemC5:neighborBed = new neighborBed("C5",["C4","C6"]);
    let itemC6:neighborBed = new neighborBed("C6",["C5","C7"]);
    let itemC7:neighborBed = new neighborBed("C7",["C6","C8"]);
    let itemC8:neighborBed = new neighborBed("C8",["C7","C9"]);
    let itemC9:neighborBed = new neighborBed("C9",["C8","C10"]);
    let itemC10:neighborBed = new neighborBed("C10",["C9"]);

    // Pod d
    let itemD1:neighborBed = new neighborBed("D1",["D2"]);
    let itemD2:neighborBed = new neighborBed("D2",["D1","D3"]);
    let itemD3:neighborBed = new neighborBed("D3",["D2","D4"]);
    let itemD4:neighborBed = new neighborBed("D4",["D3","D5"]);
    let itemD5:neighborBed = new neighborBed("D5",["D4","D6"]);
    let itemD6:neighborBed = new neighborBed("D6",["D5","D7"]);
    let itemD7:neighborBed = new neighborBed("D7",["D6"]);
    let itemD8:neighborBed = new neighborBed("D8",["D9"]);
    let itemD9:neighborBed = new neighborBed("D9",["D8","D10"]);
    let itemD10:neighborBed = new neighborBed("D10",["D9"]);



    let neighborSet:Array<neighborBed> = [itemA1, itemA2, itemA3, itemA4, itemA5,itemA6,
    									itemA7, itemA8, itemA9, itemA10, itemA11, itemA12,
    									itemB1, itemB2, itemB3, itemB4, itemB5, itemB6, itemB7,
    									itemB8, itemB9, itemB10,
    									itemC1, itemC2, itemC3, itemC4, itemC5, itemC6, itemC7,
    									itemC8, itemC9, itemC10,
    									itemD1, itemD2, itemD3, itemD4, itemD5, itemD6, itemD7,
    									itemD8, itemD9, itemD10];

    return neighborSet;
  }
}