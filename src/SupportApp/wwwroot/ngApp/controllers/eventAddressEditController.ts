namespace SupportApp.Controllers {

    export class EventAddressEditController {

        public validationErrors;
        public eventLocation;
        public eventToEdit;


        constructor(private locationIdFrom, private eventIdFrom, private eventServices: SupportApp.Services.EventServices,
            //$stateParams: angular.ui.IStateParamsService,
            private $state: angular.ui.IStateService,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance) {

            this.getLocation();
        }

        getEvent() {
            this.eventServices.getEventDetails(this.eventIdFrom).then((data) => {
                this.eventToEdit = data;
            });
        }

        getLocation() {
            this.eventServices.getLocation(this.locationIdFrom).then((data) => {
                this.eventLocation = data;
            });
        }

        saveLocation() {
            debugger;
            this.eventServices.saveLocation(this.locationIdFrom, this.eventLocation).then((data) => {
                this.eventLocation = data;
                this.close();
                //location.reload();
                this.eventToEdit = this.getEvent();
                this.$state.go("userEventEdit");
            }).catch((error) => {
                let validationErrors = [];
                for (let prop in error.data) {
                    let propErrors = error.data[prop];
                    validationErrors = validationErrors.concat(propErrors);
                }
                this.validationErrors = validationErrors;
            })
        }

        close() {
            this.$uibModalInstance.close();
        }
    }
}