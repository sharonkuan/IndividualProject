namespace SupportApp.Controllers {

    export class EventsController {

        public events;
        private selectedEventLocation;

        constructor(private eventServices: SupportApp.Services.EventServices,
            private $uibModal: ng.ui.bootstrap.IModalService) {

            this.getAllEvents();
        }

        getAllEvents() {

            this.events = this.eventServices.getAllEvents();
        }

        showDeleteDialog(eventId) {
            //debugger;
            this.$uibModal.open({
                templateUrl: '/ngApp/views/eventDeleteDialog.html',
                controller: SupportApp.Controllers.EventDeleteDialogController,
                controllerAs: 'controller',
                resolve: {  
                    eventIdFrom: () => eventId
                },
                size: 'sm'
            });
        }
    }
}