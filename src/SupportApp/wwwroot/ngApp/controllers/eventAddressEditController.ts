namespace SupportApp.Controllers {

    export class EventAddressEditController {

        public validationErrors;
        public eventLocation;


        constructor(private locationIdFrom, private eventServices: SupportApp.Services.EventServices,
            //$stateParams: angular.ui.IStateParamsService,
            //private $state: angular.ui.IStateService,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance) {

            this.getLocation();
        }

        getLocation() {
            this.eventServices.getLocation(this.locationIdFrom).then((data) => {
                this.eventLocation = data;
            });
        }

        saveLocation() {
            debugger;
            this.eventServices.saveLocation(this.locationIdFrom, this.eventLocation).then((data) => {
                this.getLocation();
                this.close();
                location.reload();
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