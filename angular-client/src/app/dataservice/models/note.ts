export class Note {
	note_id: string;
    staff_id: string;
    contents: string;
    creation_time: string;
    last_moodification_time: string;

    constructor(){
        this.contents = ''
    }
}