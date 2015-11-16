//
//  SMMessageData.h
//  SessionM
//
//  Copyright (c) 2015 SessionM. All rights reserved.
//

#import <Foundation/Foundation.h>

/*!
 @typedef SMMessageType
 @abstract Message type.
 */
typedef enum SMMessageType {
    /*! Deep links into app. */
    SMMessageTypeDeepLink = 0,
    /*! Presents full screen rich media message. */
    SMMessageTypeFullScreen,
    /*! Opens link in Safari. */
    SMMessageTypeExternalLink
} SMMessageType;


/*!
 @class SMMessageData
 @abstract Defines the data associated with a message.
 @discussion Note: the developer can configure the following properties for each message through the SessionM Mobile Marketing Cloud portal.
 */
@interface SMMessageData : NSObject

/*!
 @property actionType
 @abstract Determines how the content pointed to by @link actionURL @/link is displayed when @link //apple_ref/occ/instm/SessionM/executeMessageAction: @/link is called with an instance of this class.
 */
@property(nonatomic, readonly) SMMessageType actionType;
/*!
 @property header
 @abstract Message title.
 */
@property(nonatomic, copy, readonly) NSString *header;
/*!
 @property descriptionText
 @abstract Message text.
 */
@property(nonatomic, copy, readonly) NSString *descriptionText;
/*!
 @property actionURL
 @abstract URL for content that is displayed when @link //apple_ref/occ/instm/SessionM/executeMessageAction: @/link is called with an instance of this class.
 */
@property(nonatomic, copy, readonly) NSString *actionURL;
/*!
 @property data
 @abstract The developer's custom data associated with the message.
 */
@property(nonatomic, copy, readonly) NSDictionary *data;

/*!
 @abstract Notifies the SDK that the view containing the associated message data was presented. Used for reporting purposes.
 */
- (void)notifyPresented;
/*!
 @abstract Notifies the SDK that the user tapped on the view containing the associated message data. Used for reporting purposes.
 @discussion Removes the message data from the @link //apple_ref/occ/instp/SessionM/messagesList @/link array.
 */
- (void)notifyTapped;

@end
