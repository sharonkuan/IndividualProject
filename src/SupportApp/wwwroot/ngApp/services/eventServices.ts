namespace SupportApp.Services {

    export class EventServices {

        private eventsResources;
        private eventCommentResources;
        private eventLocationResources;

        constructor($resource: angular.resource.IResourceService) {

            this.eventsResources = $resource("/api/event/:id", null, {
                vote: {
                    method: "PUT",
                    url: "/api/event/:id"
                },
                getActiveEvents: {
                    method: 'GET',
                    url: '/api/event/getactiveevents',
                    isArray: false
                },

                getHistoryEvents: {
                    method: 'GET',
                    url: '/api/event/gethistoryevents',
                    isArray: false
                },
                getUserEvents: {
                    method: 'GET',
                    url: '/api/event/getuserevents',
                    isArray: false
                },
                getAllEvents: {
                    method: 'GET',
                    url: '/api/event/getallevents',
                    isArray: false
                }
                ,
                //event detail
                getActiveEventDetails: {
                    method: 'GET',
                    url: '/api/event/getactiveevent/:id'
                },
                getHistoryEventDetails: {
                    method: 'GET',
                    url: '/api/event/gethistoryevent/:id'
                },
                getUserEventDetails: {
                    method: 'GET',
                    url: '/api/event/getuserevent/:id'
                }
            });
            this.eventCommentResources = $resource("/api/eventComment/:id");
            this.eventLocationResources = $resource("/api/eventLocation/:id");
        }

        //this is for regular users
        //current it is not available to view until user logs in so that users can register volunteer
        getActiveEvents() {
            debugger;
            return this.eventsResources.getActiveEvents().$promise;
        }

        getHistoryEvents() {
            debugger;
            return this.eventsResources.getHistoryEvents().$promise;
        }

        getUserEvents() {

            return this.eventsResources.getUserEvents().$promise;
        }
        //this is for admin
        getAllEvents() {

            return this.eventsResources.getAllEvents().$promise;
        }

        getUserEventDetails(eventId) {
            debugger;
            return this.eventsResources.getUserEventDetails({ id: eventId }).$promise;
        }

        //this does not do any filter at all
        getActiveEventDetails(eventId) {
            return this.eventsResources.getActiveEventDetails({ id: eventId }).$promise;
        }
        
        getHistoryEventDetails(eventId) {
            return this.eventsResources.getHistoryEventDetails({ id: eventId }).$promise;
        }

        voteEvent(eventId, voteValue) {
            //alert(questionId);
            return this.eventsResources.vote({ id: eventId }, voteValue).$promise;
        }

        saveEvent(event) {

            return this.eventsResources.save(event).$promise;
        }

        //save a single event comment
        saveEventLocation(eventId, locationToSave) {
            return this.eventLocationResources.save({ id: eventId }, locationToSave).$promise;
        }

        //save a single event comment
        saveEventComment(eventId, commentToSave) {
            debugger;
            return this.eventCommentResources.save({ id: eventId }, commentToSave).$promise;
        }

        deleteEvent(eventId) {
            //debugger;
            //this.eventCommentResources.remove({ id: eventId }).$promise;
            //this.eventLocationResources.remove({ id: eventId }).$promise;
            return this.eventsResources.remove({ id: eventId }).$promise;
        }
    }

    angular.module("SupportApp").service("eventServices", EventServices);
}