namespace SupportApp.Controllers {

    export class UserEventCreateController {

        public eventToCreate;
        public validationErrors;
        public optionsOfHelp;

        public canEdit;
        public hasClaim;

        constructor(private eventServices: SupportApp.Services.EventServices,
            private $state: angular.ui.IStateService) {

            this.optionsOfHelp = ["Yes", "No", "Maybe"];
        }

        saveEvent() {
            console.log(this.eventToCreate);
            this.eventServices.saveEvent(this.eventToCreate).then((data) => {
                console.log("Saved data: " + data);
                debugger;
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