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
                search: {
                    method: 'GET',
                    url: '/api/event/search/:city',
                    isArray: true
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
                getEventDetails: {
                    method: 'GET',
                    url: '/api/event/geteventdetails/:id'
                },
                getUserEventDetails: {
                    method: 'GET',
                    url: '/api/event/getuserevent/:id'
                }
            });
            this.eventCommentResources = $resource("/api/eventComment/:id", null, {
                reloadEventDetailsPage: {
                    method: 'GET',
                    url: '/api/eventcomment/reloadeventdetailspage/:id'
                }
            });
            this.eventLocationResources = $resource("/api/eventLocation/:id");
        }

       

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
        getEventDetails(eventId) {
            return this.eventsResources.getEventDetails({ id: eventId }).$promise;
        }

        //this is for regular users
        //current it is not available to view until user logs in so that users can register volunteer
        searchByCity(city) {
            return this.eventsResources.search({ city: city }).$promise;
        }

        ////after saving an event, reload all events without adding a view count
        //reloadEventDetailsPage(eventId) {
        //    debugger;
        //    return this.eventCommentResources.reloadEventDetailsPage({ id: eventId});
        //}

        //save a single event comment
        saveEventComment(eventId, commentToSave) {
            debugger;
            return this.eventCommentResources.save({ id: eventId }, commentToSave).$promise;
        }

        voteEvent(eventId, voteValue) {
            debugger;
            return this.eventsResources.vote({ id: eventId }, voteValue).$promise;
        }

        saveEvent(event) {

            return this.eventsResources.save(event).$promise;
        }

        //save an event location
        saveEventLocation(eventId, locationToSave) {
            return this.eventLocationResources.save({ id: eventId }, locationToSave).$promise;
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