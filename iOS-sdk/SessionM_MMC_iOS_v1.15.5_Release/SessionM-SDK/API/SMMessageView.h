//
//  SMMessageView.h
//  SessionM
//
//  Copyright (c) 2016 SessionM. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "SMMessageData.h"

/*!
 @class SMMessageView
 @abstract Stores message data for presentation and provides methods for notifying the SDK that a message view was presented or tapped.
 @discussion A developer can inherit from this class to display messages with their own custom UI. A standard implementation is also provided in @link //apple_ref/occ/cl/SMDefaultMessageView @/link.
 */
@interface SMMessageView : UIView

/*!
 @property messageData
 @abstract The message data that will be presented.
 */
@property(nonatomic, strong, readonly) SMMessageData *messageData;

/*!
 @abstract Initializes a view with the given message data.
 @discussion Sets @link messageData @/link to the given instance of @link SMMessageData @/link. Once the session has started, messages can be accessed from the @link //apple_ref/occ/instp/SessionM/messagesList @/link property.
 @param data The data representing the message that will be presented.
 @result SMMessageView instance (result will be nil if <code>data</code> is nil).
 */
- (id)initWithMessageData:(SMMessageData *)data;
/*!
 @abstract Notifies the SDK that the message was presented.
 @discussion If you are inheriting from this class to use a custom presentation, be sure to call this method after the message view is presented.
 */
- (void)notifyPresented;
/*!
 @abstract Notifies the SDK that the message was tapped.
 @discussion If you are inheriting from this class to use a custom presentation, be sure to call this method after the message view is tapped.
 */
- (void)notifyTapped;

@end
