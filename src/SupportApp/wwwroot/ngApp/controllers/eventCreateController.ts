namespace SupportApp.Controllers {

    export class EventCreateController {

        public eventToCreate;
        public validationErrors;

        constructor(private eventServices: SupportApp.Services.EventServices,
            private $state: angular.ui.IStateService) {

        }

        saveEvent() {
            this.eventServices.saveEvent(this.eventToCreate).then(() => {
                this.$state.go("events");
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
            this.$state.go("events");
        }
    }
}