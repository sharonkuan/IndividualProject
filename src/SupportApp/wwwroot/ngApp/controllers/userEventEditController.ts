namespace SupportApp.Controllers {

    export class UserEventEditController {

        public eventToEdit;
        private eventId;
        public validationErrors;
        public userClaim;

        constructor(private eventServices: SupportApp.Services.EventServices,
            private accountService: SupportApp.Services.AccountService,
            $stateParams: angular.ui.IStateParamsService,
            private $state: angular.ui.IStateService,
            private $uibModal: ng.ui.bootstrap.IModalService) {

            this.userClaim = this.accountService.getUserInfo();
            this.eventId = $stateParams["id"];
            this.getEvent();
        }

        getEvent() {
            this.eventServices.getUserEventDetails(this.eventId).then((data) => {
                this.eventToEdit = data.event;
                this.eventToEdit.eventStartDate = new Date(this.eventToEdit.eventStartDate);
                this.eventToEdit.eventEndDate = new Date(this.eventToEdit.eventEndDate);
            });
        }

        saveUserEventEdit() {

            console.log(this.eventToEdit);
            this.eventServices.saveUserEventEdit(this.eventId, this.eventToEdit).then((data) => {
                this.eventToEdit = data;
                console.log(data);
                //this.$state.go("myEvents");
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

        showDeleteDialog(locationId) {
            debugger;
            this.$uibModal.open({
                templateUrl: '/ngApp/views/eventAddressEditDialog.html',
                controller: SupportApp.Controllers.EventAddressEditController,
                controllerAs: 'controller',
                resolve: {
                    locationIdFrom: () => locationId  //this eventId is passed from the form
                },
                size: 'md'
            });
        }
    }
}