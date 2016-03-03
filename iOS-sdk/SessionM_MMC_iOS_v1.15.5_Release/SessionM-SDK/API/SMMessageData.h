//
//  SMMessageData.h
//  SessionM
//
//  Copyright (c) 2016 SessionM. All rights reserved.
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
 @property messageID
 @abstract Unique ID for message.
 */
@property(nonatomic, copy, readonly) NSString *messageID;
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
 @property subheader
 @abstract Message subtitle.
 */
@property(nonatomic, copy, readonly) NSString *subheader;
/*!
 @property descriptionText
 @abstract Message text.
 */
@property(nonatomic, copy, readonly) NSString *descriptionText;
/*!
 @property iconURL
 @abstract URL for icon displayed in @link //apple_ref/occ/cl/SMActivityFeedViewCell @/link instance.
 */
@property(nonatomic, copy, readonly) NSString *iconURL;
/*!
 @property imageURL
 @abstract URL for optional banner image displayed at bottom of @link //apple_ref/occ/cl/SMActivityFeedViewCell @/link instance.
 */
@property(nonatomic, copy, readonly) NSString *imageURL;
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
 @abstract Notifies the SDK that the view containing the associated message data was presented to the user. Used for reporting purposes.
 */
- (void)notifyPresented;
/*!
 @abstract Notifies the SDK that the view containing the associated message data was seen by the user. Used for reporting purposes.
 */
- (void)notifySeen;
/*!
 @abstract Notifies the SDK that the user tapped on the view containing the associated message data. Used for reporting purposes.
 */
- (void)notifyTapped;

@end
