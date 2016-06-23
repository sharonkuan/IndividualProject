namespace SupportApp.Controllers {

    export class UserEventCreateController {

        public eventToCreate;
        public validationErrors;
        private eventId;

        public canEdit;
        public hasClaim;

        constructor(private eventServices: SupportApp.Services.EventServices,
            private $state: angular.ui.IStateService) {
        }

        saveEvent() {
            console.log(this.eventToCreate);
            this.eventServices.saveEvent(this.eventToCreate).then(() => {
                this.$state.go("myEvents");
            }).catch((error) => {
                let validationErrors = [];
                for (let prop in error.data) {
                    let propErrors = error.data[prop];
                    validationErrors = validationErrors.concat(propErrors);
                }
                this.validationErrors = validationErrors;
            })
        }

        cancel() {
            this.$state.go("myEvents");
        }
    }
}