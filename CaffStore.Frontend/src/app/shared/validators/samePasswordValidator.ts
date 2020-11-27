import { FormGroup } from '@angular/forms';

export function samePasswordValidator(fg: FormGroup) {
    return fg.get('password').value == fg.get('passwordAgain').value ? null : { 'invalid': true };
}